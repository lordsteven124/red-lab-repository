"""Un pequeño test - A simple test"""


def test_addition():
    """Test basic addition"""
    assert 2 + 2 == 4


def test_string_concatenation():
    """Test string concatenation"""
    assert "hello" + " " + "world" == "hello world"


def test_list_length():
    """Test list length"""
    test_list = [1, 2, 3, 4, 5]
    assert len(test_list) == 5


if __name__ == "__main__":
    # Run tests manually if script is executed directly
    print("Running tests...")
    
    tests = [
        (test_addition, "test_addition"),
        (test_string_concatenation, "test_string_concatenation"),
        (test_list_length, "test_list_length")
    ]
    
    for test_func, test_name in tests:
        try:
            test_func()
            print(f"✓ {test_name} passed")
        except AssertionError:
            print(f"✗ {test_name} failed")
    
    print("\nAll tests completed!")
