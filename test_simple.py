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
    
    try:
        test_addition()
        print("✓ test_addition passed")
    except AssertionError:
        print("✗ test_addition failed")
    
    try:
        test_string_concatenation()
        print("✓ test_string_concatenation passed")
    except AssertionError:
        print("✗ test_string_concatenation failed")
    
    try:
        test_list_length()
        print("✓ test_list_length passed")
    except AssertionError:
        print("✗ test_list_length failed")
    
    print("\nAll tests completed!")
